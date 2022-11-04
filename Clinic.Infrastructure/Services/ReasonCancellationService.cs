using AutoMapper;
using Clinic.Core.Dtos;
using Clinic.Core.Helper;
using Clinic.Data.Data;
using Clinic.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Services
{
    public class ReasonCancellationService : IReasonCancellationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ReasonCancellationService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<(List<ReasonCancellation> ReasonCancellations, int CountItems)> GetReasonCancellations(int skip, int take, string s = null)
        {
            var query = _db.ReasonsCancellation
                .Where(
                    x => (s == null
                    || x.Title.Contains(s)
                    || x.Note.Contains(s)))
                .OrderBy(x => x.CreatedAt);
            return (await query.Skip(skip).Take(take).ToListAsync(), await query.CountAsync());
        }

        public async Task<bool> Delete(int id)
        {
            var item = await GetReasonCancellation(id);
            item.IsDelete = true;
            _db.ReasonsCancellation.Update(item);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ReasonCancellationDto> Get(int id)
        {
            return _mapper.Map<ReasonCancellationDto>(await GetReasonCancellation(id));
        }

        public async Task<ReasonCancellation> AddReasonCancellation(string userId, ReasonCancellationDto model)
        {
            var item = _mapper.Map(model, new ReasonCancellation(userId));
            await _db.ReasonsCancellation.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<ReasonCancellation> UpdateReasonCancellation(string userId, int id, ReasonCancellationDto model)
        {
            var item = await GetReasonCancellation(id);
            _mapper.Map(model, item);
            item.UpdatedBy = userId;
            item.UpdatedAt = DateTime.Now;
            _db.ReasonsCancellation.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }


        private async Task<ReasonCancellation> GetReasonCancellation(int id)
        {
            var item = await _db.ReasonsCancellation.SingleOrDefaultAsync(x => x.Id == id);
            if (item is null)
                throw new NotFoundException(id);
            return item;
        }
    }
    public interface IReasonCancellationService
    {
        Task<ReasonCancellation> AddReasonCancellation(string userId, ReasonCancellationDto model);
        Task<bool> Delete(int id);
        Task<ReasonCancellationDto> Get(int id);
        Task<(List<ReasonCancellation> ReasonCancellations, int CountItems)> GetReasonCancellations(int skip, int take, string s = null);
        Task<ReasonCancellation> UpdateReasonCancellation(string userId, int id, ReasonCancellationDto model);
    }
}

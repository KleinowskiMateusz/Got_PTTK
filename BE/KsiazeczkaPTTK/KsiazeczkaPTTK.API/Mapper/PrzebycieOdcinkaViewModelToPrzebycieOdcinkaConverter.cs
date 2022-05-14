using AutoMapper;
using KsiazeczkaPttk.API.ViewModels;
using KsiazeczkaPttk.Domain.Models;

namespace KsiazeczkaPttk.API.Mapper
{
    public class PrzebycieOdcinkaViewModelToPrzebycieOdcinkaConverter : IValueConverter<IEnumerable<PrzebycieOdcinkaViewModel>, IEnumerable<SegmentTravel>>
    {
        public IEnumerable<SegmentTravel> Convert(IEnumerable<PrzebycieOdcinkaViewModel> sourceMembers, ResolutionContext context)
        {
            return sourceMembers.Select(member => new SegmentTravel
            {
                Order = member.Kolejnosc,
                IsBack = member.Powrot,
                SegmentId = member.OdcinekId,
            });
        }
    }
}

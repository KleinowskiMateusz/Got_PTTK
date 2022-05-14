using AutoMapper;
using KsiazeczkaPttk.API.ViewModels;
using KsiazeczkaPttk.Domain.Models;

namespace KsiazeczkaPttk.API.Mapper
{
    public class SegmentTravelViewModelToSegmentTravelConverter : IValueConverter<IEnumerable<SegmentTravelViewModel>, IEnumerable<SegmentTravel>>
    {
        public IEnumerable<SegmentTravel> Convert(IEnumerable<SegmentTravelViewModel> sourceMembers, ResolutionContext context)
        {
            return sourceMembers.Select(member => new SegmentTravel
            {
                Order = member.Order,
                IsBack = member.IsBack,
                SegmentId = member.SegmentId,
            });
        }
    }
}

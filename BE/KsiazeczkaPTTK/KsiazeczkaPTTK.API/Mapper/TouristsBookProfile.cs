using AutoMapper;
using KsiazeczkaPttk.API.ViewModels;
using KsiazeczkaPttk.Domain.Enums;
using KsiazeczkaPttk.Domain.Models;
using System;

namespace KsiazeczkaPttk.API.Mapper
{
    public class TouristsBookProfile : Profile
    {
        public TouristsBookProfile()
        {
            CreateOdcinekMapping();

            CreateMap<CreatePunktTerenowyViewModel, TerrainPoint>();

            CreatePotwierdzenieMapping();
            
            CreateWycieczkaMapping();
        }

        private void CreateOdcinekMapping()
        {
            CreateMap<CreateOdcinekPublicznyViewModel, Segment>()
                .ForMember(m => m.Version, o => o.MapFrom(_ => 1));

            CreateMap<EditOdcinekPublicznyViewModel, Segment>();

            CreateMap<CreateOdcinekViewModel, Segment>()
                .ForMember(m => m.Version, o => o.MapFrom(_ => 1));

            CreateMap<Segment, NeighboringSegment>();
        }

        private void CreatePotwierdzenieMapping()
        {
            CreateMap<CreatePotwierdzenieWithQrViewModel, Confirmation>()
                .ForMember(m => m.Type, opt => opt.MapFrom(_ => ConfirmationType.QrCode))
                .ForMember(m => m.TerrainPointId, opt => opt.MapFrom(src => src.PunktId))
                .ForMember(m => m.IsAdministration, opt => opt.MapFrom(_ => false));

            CreateMap<CreatePotwierdzenieWithImageViewModel, Confirmation>()
                .ForMember(m => m.Type, opt => opt.MapFrom(_ => ConfirmationType.Image))
                .ForMember(m => m.TerrainPointId, opt => opt.MapFrom(src => src.PunktId))
                .ForMember(m => m.IsAdministration, opt => opt.MapFrom(_ => false));

        }

        private void CreateWycieczkaMapping()
        {
            CreateMap<CreateWycieczkaViewModel, Trip>()
                .ForMember(m => m.Segments, opt => opt.ConvertUsing(new PrzebycieOdcinkaViewModelToPrzebycieOdcinkaConverter(), src => src.PrzebyteOdcinki));
        }
    }
}

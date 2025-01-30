using AutoMapper;
using Backend_Crypto.Dto;
using Backend_Crypto.Models;

namespace Backend_Crypto.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Crypto,CryptoDto>();
            CreateMap<HistoriquePrix,HistoriqueDto>();
            CreateMap<CryptoUpdateDto, Crypto>();
            CreateMap<StockPortefeuille, StockDto>();
            CreateMap<Transaction,TransactionDto>()
                .ForMember(trans => trans.Type , opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(trans => trans.State, opt => opt.MapFrom(src => src.State.ToString()));
            CreateMap<PortefeuilleDto,Portefeuille>();
            CreateMap<Portefeuille, PortefeuilleDto>();
        }
    }
}

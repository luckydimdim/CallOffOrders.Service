using AutoMapper;
using Cmas.BusinessLayers.CallOffOrders.Entities;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using Cmas.Services.CallOffOrders.Dtos.Responses;

namespace Cmas.Services.CallOffOrders
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateRateRequest, Rate>();
            CreateMap<UpdateRateRequest, Rate>();
            CreateMap<UpdateCallOffOrderRequest, CallOffOrder>();

            CreateMap<CallOffOrder, DetailedCallOffOrderResponse>();
            CreateMap<CallOffOrder, SimpleCallOffOrderResponse>();
            CreateMap<Rate, RateResponse>();
        }
    }

}

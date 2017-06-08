using AutoMapper;
using Cmas.BusinessLayers.CallOffOrders.Entities;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using Cmas.Services.CallOffOrders.Dtos.Responses;
using System;

namespace Cmas.Services.CallOffOrders
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateRateRequest, Rate>();
            CreateMap<RateRequest, Rate>();
            CreateMap<UpdateCallOffOrderRequest, CallOffOrder>();
            CreateMap<CreateCallOffOrderRequest, CallOffOrder>();
            CreateMap<CallOffOrder, DetailedCallOffOrderResponse>();
            CreateMap<CallOffOrder, SimpleCallOffOrderResponse>();
            CreateMap<Rate, RateResponse>();

            CreateMap<RateUnit, int>().ConvertUsing(src => (int)src);
            CreateMap<int, RateUnit>()
                .ConvertUsing(src => (RateUnit)Enum.Parse(typeof(RateUnit), src.ToString()));
        }
    }

}

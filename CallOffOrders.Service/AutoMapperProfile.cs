using AutoMapper;
using Cmas.BusinessLayers.CallOffOrders.Entities;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using Cmas.Services.CallOffOrders.Dtos.Responses;
using System;

namespace Cmas.Services.CallOffOrders
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Округление до двух знаков после запятой
        /// Округление происходит к тому числу, которое дальше от нуля
        /// </summary>
        public double RoundDoubleTwo(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public AutoMapperProfile()
        {
            CreateMap<UpdateRateRequest, Rate>()
                .ForMember(item => item.Amount, opt => opt.ResolveUsing(i => RoundDoubleTwo(i.Amount)));

            CreateMap<RateRequest, Rate>()
                .ForMember(item => item.Amount, opt => opt.ResolveUsing(i => RoundDoubleTwo(i.Amount)));

            CreateMap<UpdateCallOffOrderRequest, CallOffOrder>();
            CreateMap<CreateCallOffOrderRequest, CallOffOrder>();
            CreateMap<CallOffOrder, DetailedCallOffOrderResponse>();
            CreateMap<CallOffOrder, SimpleCallOffOrderResponse>();
            CreateMap<Rate, RateResponse>();

            CreateMap<RateUnit, int>().ConvertUsing(src => (int) src);
            CreateMap<int, RateUnit>()
                .ConvertUsing(src => (RateUnit) Enum.Parse(typeof(RateUnit), src.ToString()));
        }
    }
}
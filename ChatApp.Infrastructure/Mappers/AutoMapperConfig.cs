using AutoMapper;
using ChatApp.Core.Domain;
using ChatApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EComerence.Infrastructure.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, AccountDto>();

            })
            .CreateMapper();
    }
}

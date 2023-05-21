using AutoMapper;
using Inspira_Libertad.DTOs;
using Inspira_Libertad.Models;

namespace Inspira_Libertad.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ArticuloDTO, Articulo>().ReverseMap().ForMember(p => p.File, options => options.Ignore());
            CreateMap<CursoDTO, Curso>().ReverseMap().ForMember(p => p.File, options => options.Ignore());
        }
    }
}

using destapp.apiClient.Models;
using destapp.biz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace destapp.api.Models.Response
{
    public class PerfilResponse
    {
        public Perfil data { get; set; }

        public class Perfil
        {
            public ListGenderDb gender { get; set; }
            public ListEstadosPai states { get; set; }
            public ListCitiesState cities { get; set; }
            public AvatarResponse avatar { get; set; }
            public InteresesResponse interest { get; set; }
            public BadgesResponse badges { get; set; }

            public class ListGenderDb
            {
                public List<GenderDb> data { get; set; }
            }

            public class ListCitiesState {
                public List<CitiesState> data { get; set; }
            }

            public class ListEstadosPai
            {
                public List<EstadosPai> data { get; set; }
            }

            public Perfil()
            {
                gender = new ListGenderDb();
                states = new ListEstadosPai();
                cities = new ListCitiesState();
                avatar = new AvatarResponse();
                interest = new InteresesResponse();
                badges = new BadgesResponse();
            }
        }

        public PerfilResponse()
        {
            data = new Perfil();
        }
    }
}

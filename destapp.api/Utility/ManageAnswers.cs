using destapp.api.Controllers;
using destapp.apiClient.Models.Trivia;
using destapp.biz.Entities;
using destapp.dal.db_context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static destapp.apiClient.Models.Trivia.Question;
using static destapp.apiClient.Models.Trivia.TriviaIdResponse;

namespace destapp.api.Utility
{
    public class ManageAnswers
    {
        private readonly Db_DestappContext context;

        public ManageAnswers(Db_DestappContext _context)
        {
            this.context = _context;
        }

        public async Task<List<TriviaIntentoRespuesta>> get_respuestas_correctasAsync(List<TriviaIntentoRespuesta> trivia_ans, TriviaIdResponse triviaResponse)
        {
            List<TriviaIntentoRespuesta> ans_val = trivia_ans;
            ///////////////////////////// SE TRAEN RESPUESTAS DEL CMS  //////////////////////////////////

            //apiClient.CoreApiClient.Trivia triviaCms = new apiClient.CoreApiClient.Trivia();
            //TriviaIdResponse triviaResponse = await triviaCms.getAllTrivias_byid(idtrivia.ToString());
            var questions = triviaResponse.data.questions;

            foreach (TriviaIntentoRespuesta AU in ans_val)
            {
                id_Question Q_ = triviaResponse.data.questions.FirstOrDefault(q => q.question_id.id == -100);
                id_Question Q = triviaResponse.data.questions.FirstOrDefault(q => q.question_id.id == AU.id);
                if (Q != null)
                {
                    int numero;
                    if (Int32.TryParse(AU.answer, out numero)) //si es número es de opción multiple
                    {
                        //se busca la respuesta  en la pregunta
                        id_Answer A = Q.question_id.answers.FirstOrDefault(a => a.answer_id.id == Convert.ToInt32(AU.answer));
                        if (Convert.ToBoolean(A.answer_id.correct_answer))
                            AU.status = true;
                        else
                            AU.status = false;
                    }
                    else
                    {
                        //las abiertas sob siempre correctas
                        AU.status = true;
                    }

                }
            }

            return ans_val;
        }


    }
}

namespace Epic_Arts_Entertainment.Models
{
    public class AtendimentoVM
    {
        public int IdAtendimento { get; set; }

        public DateTime DtHoraAgendamento { get; set; }

        public DateOnly DataAtendimento { get; set; }

        public TimeOnly Horario { get; set; }

        public int IdUsuario { get; set; }

        public int IdServico { get; set; }
    }
}

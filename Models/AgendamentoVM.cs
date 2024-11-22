namespace Epic_Arts_Entertainment.Models
{
    public class AgendamentoVM
    {
        public int IdAgendamento { get; set; }

        public DateTime DtHoraAgendamento { get; set; }

        public DateOnly DataAgendamento { get; set; }

        public TimeOnly Horario { get; set; }

        public int IdUsuario { get; set; }

        public int IdServico { get; set; }
    }
}

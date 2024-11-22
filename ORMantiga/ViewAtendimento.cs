using System;
using System.Collections.Generic;

namespace Epic_Arts_Entertainment.ORM;

public partial class ViewAtendimento
{
    public int IdAtendimento { get; set; }

    public DateTime DtHoraAgendamento { get; set; }

    public DateOnly DataAtendimento { get; set; }

    public TimeOnly Horario { get; set; }

    public string TipoServico { get; set; } = null!;

    public decimal Valor { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefone { get; set; } = null!;
}

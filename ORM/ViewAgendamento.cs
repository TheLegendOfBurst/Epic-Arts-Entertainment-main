﻿using System;
using System.Collections.Generic;

namespace Epic_Arts_Entertainment.ORM1;

public partial class ViewAgendamento
{
    public string TipoServico { get; set; } = null!;

    public decimal Valor { get; set; }

    public DateTime DtHoraAgendamento { get; set; }

    public DateOnly DataAgendamento { get; set; }

    public TimeOnly Horario { get; set; }

    public int IdAgendamento { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefone { get; set; } = null!;
}

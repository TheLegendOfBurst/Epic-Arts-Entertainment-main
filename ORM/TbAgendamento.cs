using System;
using System.Collections.Generic;

namespace Epic_Arts_Entertainment.ORM;

public partial class TbAgendamento
{
    public int IdAgendamento { get; set; }

    public DateTime DtHoraAgendamento { get; set; }

    public DateOnly DataAgendamento { get; set; }

    public TimeOnly Horario { get; set; }

    public int IdUsuario { get; set; }

    public int IdServico { get; set; }

    public virtual TbServico IdServicoNavigation { get; set; } = null!;

    public virtual TbUsuario IdUsuarioNavigation { get; set; } = null!;
}

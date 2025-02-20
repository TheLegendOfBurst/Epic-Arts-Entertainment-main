﻿using Epic_Arts_Entertainment.ORM;

namespace Epic_Arts_Entertainment.Models
{
    public class AgendamentoVM
    {
        public int IdAgendamento { get; set; }
        public DateTime DtHoraAgendamento { get; set; }
        public DateOnly DataAgendamento { get; set; }
        public TimeOnly Horario { get; set; }

        // Propriedades adicionais para o nome e email do usuário
        public string? UsuarioNome { get; set; }
        public string? UsuarioEmail { get; set; }  // Adicionando a propriedade UsuarioEmail
        public string? UsuarioTelefone { get; set; }  // Telefone do usuário (adicionado)
        public string? ServicoNome { get; set; }
        public decimal ServicoValor { get; set; }  // Preço do serviço

        // Propriedades adicionais
        public int IdUsuario { get; set; }
        public int IdServico { get; set; }

        // Propriedades de formatação
        public string DtHoraAgendamentoFormatada => DtHoraAgendamento.ToString("dd/MM/yyyy HH:mm");
        public string DataAtendimentoFormatada => DataAgendamento.ToString("dd/MM/yyyy");
        public string HorarioFormatado => Horario.ToString("HH:mm");

        // Propriedades para navegação
        public UsuarioVM? Usuario { get; set; }
        public ServicoVM? Servico { get; set; }
    }
}
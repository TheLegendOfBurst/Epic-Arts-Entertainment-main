@{
    Layout = "~/Views/Shared/_Layout_Gerenciamento.cshtml";
}

<body>

    <section class="maneger-page-section">
        <div class="container page-maneger servico-height">
            <!-- Cabeçalho com logo e nome da empresa -->
            <div class="logo-container text-center mt-3 mb-3">
                <a href="/Home/Index">
                    <img src="~/images/Logo/Logo.png" alt="MIGHTVR Logo" style="width: 100px; height: 100px;">
                </a>
            </div>

            <!-- Titulo -->
            <div class="container">
                <div class="modal-header" style="background-color: #000000; color: white; border-color: #fc0fc0;">
                    <h1 class="modal-title" id="exampleModalLabel" style="color: #fc0fc0;">Adicionar Agendamento</h1>
                </div>

                <div class="modal-body">
                    <form>
                        <div class="mb-3 row">
                            <div class="col-md-6">
                                <input id="data_agdt" type="date" class="form-control" placeholder="Digite a data do agendamento">
                            </div>
                            <div class="col-md-6">
                                <span style="margin-top: 7px; display: inline-block; font-size: 20px; color: #ffe100;">*</span> <!-- Ajuste do asterisco -->
                            </div>
                        </div>

                        <div class="mb-3 row">
                            <div class="col-md-6">
                                <select id="Servico" class="form-select">
                                    <option value="">Escolha o Serviço</option>
                                    <option value="VolumeBrasileiro">Desenvolver Jogos</option>
                                    <option value="Volume5D">Design de personagens e ambientes</option>
                                    <option value="EfeitoSirene">Teste de Qualidade e Garantia</option>
                                    <option value="VolumeExpress">Criação de Roteiro e Narrativa</option>
                                    <option value="HidraGloss">Marketing e Lançamento de Jogos</option>
                                    <option value="Suporte">Suporte e Manutenção de Jogos e Consoles</option>
                                </select>
                            </div>
                        </div>

                        <!-- Checkboxes de horários com texto branco -->
                        <div class="mb-3 d-flex align-items-center" style="color: white;">
                            <input type="checkbox" id="chk08M" class="form-check-input" value="08:00" style="margin-right: 10px;" data-group="horario">
                            <label class="form-check-label" for="chk08">Atendimento das 8:00</label>
                            <span id="Span08M" style="color:red;"></span>
                        </div>

                        <div class="mb-3 d-flex align-items-center" style="color: white;">
                            <input type="checkbox" id="chk10M" class="form-check-input" value="10:00" style="margin-right: 10px;" data-group="horario">
                            <label class="form-check-label" for="chk10">Atendimento das 10:00</label>
                            <span id="Span10M" style="color:red;"></span>
                        </div>

                        <div class="mb-3 d-flex align-items-center" style="color: white;">
                            <input type="checkbox" id="chk13M" class="form-check-input" value="13:00" style="margin-right: 10px;" data-group="horario">
                            <label class="form-check-label" for="chk13">Atendimento das 13:00</label>
                            <span id="Span13M" style="color:red;"></span>
                        </div>

                        <div class="mb-3 d-flex align-items-center" style="color: white;">
                            <input type="checkbox" id="chk15M" class="form-check-input" value="15:00" style="margin-right: 10px;" data-group="horario">
                            <label class="form-check-label" for="chk15">Atendimento das 15:00</label>
                            <span id="Span15M" style="color:red;"></span>
                        </div>

                        <div class="mb-3 d-flex align-items-center" style="color: white;">
                            <input type="checkbox" id="chk17M" class="form-check-input" value="17:00" style="margin-right: 10px;" data-group="horario">
                            <label class="form-check-label" for="chk17">Atendimento das 17:00</label>
                            <span id="Span17M" style="color:red;"></span>
                        </div>

                        <div class="mb-3 d-flex align-items-center" style="color: white;">
                            <input type="checkbox" id="chk19M" class="form-check-input" value="19:00" style="margin-right: 10px;" data-group="horario">
                            <label class="form-check-label" for="chk19">Atendimento das 19:00</label>
                            <span id="Span19M" style="color:red;"></span>
                        </div>
                        <!-- Fim dos checkboxes de horários -->

                        <input type="hidden" id="IdEdt" value="">
                    </form>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="InserirAgendamento()">Adicionar</button>
                </div>
            </div> <!-- Fechando div.modal-body -->
        </div> <!-- Fechando div.container -->
    </section> <!-- Fechando section.maneger-page-section -->

    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script>
    $(document).ready(function () {
        // Impede a digitação manual no campo de data
        document.getElementById('data_agdt').addEventListener('keydown', function (e) {
            e.preventDefault(); // Bloqueia qualquer entrada do teclado
        });

        // Função para ativar/desativar os checkboxes
        $('input[type="checkbox"]').click(function () {
            var isChecked = $(this).prop("checked");
            var groupId = $(this).attr("data-group");

            if (isChecked) {
                $('input[type="checkbox"][data-group="' + groupId + '"]').not($(this)).prop("checked", false);
            }
        });
    });

    function consultarAgendamento() {
        ativarCheckboxes();
        limparTextosSpans();
        desmarcarCheckboxes();

        var data = $("#data_agdt").val();
        $.ajax({
            type: "GET",
            url: "/Agendamento/ConsultarAgendamento",
            data: { data: data },
            success: function (response) {
                response.forEach(function (item) {
                    switch (item.horario) {
                        case "08:00:00":
                            $("#chk08M").prop("disabled", true);
                            $("#Span08M").text("Horario Indisponivel");
                            break;
                        case "10:00:00":
                            $("#chk10M").prop("disabled", true);
                            $("#Span10M").text("Horario Indisponivel");
                            break;
                        case "13:00:00":
                            $("#chk13M").prop("disabled", true);
                            $("#Span13M").text("Horario Indisponivel");
                            break;
                        case "15:00:00":
                            $("#chk15M").prop("disabled", true);
                            $("#Span15M").text("Horario Indisponivel");
                            break;
                        case "17:00:00":
                            $("#chk17M").prop("disabled", true);
                            $("#Span17M").text("Horario Indisponivel");
                            break;
                        case "19:00:00":
                            $("#chk19M").prop("disabled", true);
                            $("#Span19M").text("Horario Indisponivel");
                            break;
                        default:
                            break;
                    }
                });
            },
            error: function (error) {
                console.error("Erro na requisição AJAX:", error);
            }
        });
    }

    function ativarCheckboxes() {
        var checkboxes = document.querySelectorAll('input[type="checkbox"]');
        checkboxes.forEach(function (checkbox) {
            checkbox.disabled = false;
        });
    }

    function limparTextosSpans() {
        var spans = document.querySelectorAll('form span');
        spans.forEach(function (span) {
            span.textContent = '';
        });
    }

    function desmarcarCheckboxes() {
        var checkboxes = document.querySelectorAll('form input[type="checkbox"]');
        checkboxes.forEach(function (checkbox) {
            checkbox.checked = false;
        });
    }

    function obterDataHoraFormatada() {
        var dataHoraAtual = new Date();
        var dataHoraFormatada = dataHoraAtual.getFullYear() + "-" +
            ("0" + (dataHoraAtual.getMonth() + 1)).slice(-2) + "-" +
            ("0" + dataHoraAtual.getDate()).slice(-2) + " " +
            ("0" + dataHoraAtual.getHours()).slice(-2) + ":" +
            ("0" + dataHoraAtual.getMinutes()).slice(-2) + ":" +
            ("0" + dataHoraAtual.getSeconds()).slice(-2);

        return dataHoraFormatada;
    }

    function InserirAgendamento() {
        var data = $("#data_agdt").val();
        var servico = $("#Servico").val();
        var horarioSelecionado = $("input[type='checkbox']:checked").val();
        var dataHora = obterDataHoraFormatada();

        if (!data || !servico || !horarioSelecionado) {
            alert("Preencha todos os campos!");
            return;
        }

        $.ajax({
            type: "POST",
            url: "/Agendamento/InserirAgendamento",
            data: {
                Data: data,
                Servico: servico,
                Horario: horarioSelecionado,
                DataHora: dataHora
            },
            success: function (response) {
                alert("Agendamento inserido com sucesso!");
                window.location.reload();
            },
            error: function (error) {
                console.error("Erro ao inserir agendamento:", error);
            }
        });
    }
    </script>

</body>

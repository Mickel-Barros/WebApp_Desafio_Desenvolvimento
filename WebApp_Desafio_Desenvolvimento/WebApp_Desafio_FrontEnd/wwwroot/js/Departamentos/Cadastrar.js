$(document).ready(function () {

    $('#btnCancelar').click(function () {
        Swal.fire({
            html: "Deseja cancelar essa operação? O registro não será salvo.",
            type: "warning",
            showCancelButton: true,
        }).then(function (result) {
            if (result.value) {
                history.back();
            } else {
                console.log("Cancelou a inclusão.");
            }
        });
    });

    $('#btnSalvar').click(function () {

        if ($('#form').valid() != true) {
            FormularioInvalidoAlert();
            return;
        }

        let departamento = SerielizeForm($('#form'));
        let url = $('#form').attr('action');
        //debugger;

        $.ajax({
            type: "POST",
            url: url,
            data: departamento,
            success: function (result) {
                let mensagem = departamento.ID !== "0" ? "Departamento editado com sucesso!" : result.Message;
                Swal.fire({
                    type: result.Type,
                    title: result.Title,
                    text: mensagem,
                }).then(function () {
                    window.location.href = config.contextPath + result.Controller + '/' + result.Action;
                });

            },
            error: function (result) {

                Swal.fire({
                    text: result.statusText,
                    confirmButtonText: 'OK',
                    icon: 'error'
                });

            },
        });
    });

});

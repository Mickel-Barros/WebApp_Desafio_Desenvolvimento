﻿$(document).ready(function () {

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

        let dataAtual = new Date(Date.now());
        let dataSelecionada = $('#DataAbertura.form-control');
        let dataFormatada = new Date(dataSelecionada.val());
        let dataSeguinte = new Date(dataFormatada.getFullYear(), dataFormatada.getMonth(), dataFormatada.getDate() + 2);

        if (dataSeguinte < dataAtual) {
            Swal.fire({
                text: "A data selecionada é retroativa. Por favor, escolha uma data atual ou futura.",
                confirmButtonText: 'OK',
                icon: 'error'
            });
            return;
        }

        let chamado = SerielizeForm($('#form'));
        let url = $('#form').attr('action');
        //debugger;

        $.ajax({
            type: "POST",
            url: url,
            data: chamado,
            success: function (result) {
                let mensagem = chamado.ID !== "0" ? "Chamado editado com sucesso!" : result.Message;
                Swal.fire({
                    type: result.Type,
                    title: result.Title,
                    text: mensagem
                }).then(function () {
                    window.location.href = config.contextPath + result.Controller + '/' + result.Action;
                });

            },
            error: function (result) {
                Swal.fire({
                    text: result.responseJSON.Message,
                    confirmButtonText: 'OK',
                    icon: 'error'
                });

            },
        });
    });

});

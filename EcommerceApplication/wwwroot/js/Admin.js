$(() => {

    $("#add-product").on('click', function () {

        $("#modal-add").modal('show');
    })

    $("#add").on('click', function () {

        $("#modal-add").modal('hide');
    })

})
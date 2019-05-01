$(() => {

    $("#add-to-cart").on('click', function () {

        $.post("/home/addcart", { itemId: $(this).data('itemid'), quantity: $("#quantity").val() }, function () {

            alert('added');
        })

    })



})

var CategoryFormVew = function () {

    var that = this;

    this.init = function () {
        this.loadCategories();
        $('#btn-add').on('click', that.showForm);
        $('#btn-submit').on('click', that.submitForm);
    };

    this.loadCategories = function () {
        var request = $.ajax({
            url: "/category/list",
            method: "GET",
            dataType: 'html'
        });

        request.done(function (response) {
            //debugger;
            $('#category-list').html(response);
        });

        request.fail(function (xhr, textStatus) {
            alert('error: ' + xhr.responseText);
        });
    };

    this.showError = function (error) {
        //debugger;
        var err = error;
        if ($.isArray(err)) {
            err = Collections.displayList(err);
        }
        $("#msg-error").html(err);
        $("#msg-error").removeClass('hidden');
    };

    this.showForm = function () {
        $("#msg-error").addClass('hidden');
        $('#dlg-add').modal('show');
    };

    this.submitForm = function () {
        $("#msg-error").addClass('hidden');
        var formData = {
            name: $('#name').val(),
            description: $('#description').val(),
        };
        var request = $.ajax({
            url: "/category",
            method: "POST",
            data: formData,
            dataType: 'json'
        });

        request.done(function (response) {
            //debugger;
            if (response.success) {
                $('#dlg-add').modal('hide');
                that.loadCategories();
            }
            else {
                that.showError(response.messages);
            }
        });

        request.fail(function (xhr, textStatus) {
            that.showError(xhr.responseText);
        });
    };

}


$(document).ready(function()
{
    var cfv = new CategoryFormVew();
    cfv.init();
});

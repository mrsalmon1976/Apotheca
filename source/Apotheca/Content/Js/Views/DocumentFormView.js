Dropzone.autoDiscover = false;

var DocumentFormVew = function () {

    var that = this;
    this.dropZone = null;
    this.uploadedFileName = '';

    this.init = function () {
        this.dropZone = new Dropzone("div#dropzone", {
            url: "/document/upload",
            maxFiles: 1,
            addRemoveLinks: true,
            renameFilename: function (name) {
                that.uploadedFileName = Numeric.getRandomInt(10000, 100000).toString() + '_' + name;
                return that.uploadedFileName;
            }
        });
        this.dropZone.on("addedfile", function () {
            if (this.files[1] != null) {
                this.removeFile(this.files[0]);
            }
        });
        this.dropZone.on("removedfile", function () {
            that.toggleFormState('', false);
        });
        this.dropZone.on('success', function (file) {
            that.toggleFormState(file.name, true);
        });
    };

    this.toggleFormState = function (fileName, isSubmitEnabled) {
        $('#txt-name').val(fileName);
        $('#btn-submit-document').prop('disabled', !isSubmitEnabled);
        $('#hid-filename').val(that.uploadedFileName);
    };
}


$(document).ready(function()
{
    var d = new DocumentFormVew();
    d.init();
});

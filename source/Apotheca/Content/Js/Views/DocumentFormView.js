Dropzone.autoDiscover = false;

var DocumentFormVew = function () {

    var that = this;
    this.dropZone = null;
    this.uploadedFileName = '';
    this.selectedCategories = [];

    this.addCategory = function (val, text) {
        this.selectedCategories.push({
            key: val,
            value: text
        });
        this.updateCategoryNames();
    };

    this.init = function () {
        // set up the dropzone
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

        // set up the checkboxes
        $('.category-checkbox').each(function () {
            var chk = $(this);
            if (chk.is(':checked')) {
                that.addCategory($(this).val(), $(this).attr('data-text'));
            }
        });
        $(".category-checkbox").on('change', function () {
            if (this.checked) {
                //debugger;
                that.addCategory(this.value, this.attributes['data-text'].value);
            }
            else {
                that.removeCategory(this.value);
            }
        });
        
    };

    this.removeCategory = function(id) {
        var index = -1;
        for (var i=0; i<this.selectedCategories.length; i++) {
            if (this.selectedCategories[i].key == id) {
                this.selectedCategories.splice(i, 1);
            }
        }
        this.updateCategoryNames();
    };

    this.toggleFormState = function (fileName, isSubmitEnabled) {
        $('#txt-name').val(fileName);
        $('#btn-submit-document').prop('disabled', !isSubmitEnabled);
        $('#hid-filename').val(that.uploadedFileName);
    };

    this.updateCategoryNames = function() {
        var text = 'No categories selected';
        var items = [];
        if (this.selectedCategories.length > 0) {
            for (var i = 0; i < this.selectedCategories.length; i++) {
                items.push(this.selectedCategories[i].value);
            }
            text = items.join(', ');
        }
        $('#category-names').html(text);
    };
}


$(document).ready(function()
{
    var d = new DocumentFormVew();
    d.init();
});

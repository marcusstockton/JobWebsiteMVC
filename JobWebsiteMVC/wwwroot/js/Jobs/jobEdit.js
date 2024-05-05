$(function () {

    var descriptionPreviewString = $('#descriptionInput').val();
    document.getElementById('preview').innerHTML =
        marked.parse(descriptionPreviewString);

    $('#descriptionInput').on("keyup", function () {
        descriptionPreviewString = this.value;
        document.getElementById('preview').innerHTML =
            marked.parse(descriptionPreviewString);
    })

    $('#previewMarkdownSwitch').on("change", function () {
        this.checked ? $('#preview').show() : $('#preview').hide();
    });
});
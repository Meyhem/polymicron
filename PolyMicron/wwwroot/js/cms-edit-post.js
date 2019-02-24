$(function () {
    function mirrorQuill(quill) {
        $('#quill-mirror').val(quill.container.querySelector('.ql-editor').innerHTML);
    }

    var toolbarOptions = [
        [{ 'header': 1 }, { 'header': 2 }],
        ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
        [{ 'color': [] }, { 'background': [] }],
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
        ['link', 'image', 'video'],
        ['blockquote', 'code-block'],
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
        [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
        [{ 'direction': 'rtl' }],                         // text direction
        [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
        // [{ 'font': ['roboto'] }],
        [{ 'align': [] }],
        ['clean']                                         // remove formatting button
    ];

    var font = Quill.import('formats/font');
    font.whitelist = ['roboto'];

    Quill.register(font, true);

    var quill = new Quill('.quill-container', {
        placeholder: 'Write your article',
        theme: 'snow',
        modules: {
            toolbar: toolbarOptions
        }
    });

    mirrorQuill(quill);

    quill.on('text-change', function () {
        mirrorQuill(quill);
    });

    tagsInput($('input[type=tags]')[0]);
});

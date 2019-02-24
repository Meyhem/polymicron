$(function () {
    $('.button-delete-post').click(function () {
        var $this = $(this);
        var postId = $this.data('post-id');

        if (isNaN(Number(postId))) {
            return console.error('postid is undefined, cannot open modal');
        }

        $('#delete-post-modal-' + postId).addClass('is-active');

    });

    $('.button-add-thumbnail').click(function () {
        var $this = $(this);
        var postId = $this.data('post-id');
        if (isNaN(Number(postId))) {
            return console.error('postid is undefined, cannot open modal');
        }

        $('#add-thumbnail-modal-' + postId).addClass('is-active');
    });

    $('.delete-post-close, .add-thumbnail-close').click(function () {
        $(this).closest('.modal').removeClass('is-active');
    });
});
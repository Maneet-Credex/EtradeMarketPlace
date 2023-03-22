function updateChatBox() {
    var receiverId = $('#chat-box').data('receiver-id');
    console.log(receiverId);
    $.ajax({
        url: `/Messages/GetSenderChat?ReceiverId=${receiverId}`,
        type: 'GET',
        success: function (result) {
            console.log(result);
            $('#chat-box').html(result);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

setInterval(updateChatBox, 15000);

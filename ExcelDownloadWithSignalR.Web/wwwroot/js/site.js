var toastTimeout;

$(document).ready(function () {

    const connection = new window.signalR.HubConnectionBuilder().withUrl("/hub").build();

    connection.start().then(() => { console.log("Connection successful") })

    connection.on("AlertCompleteFile", (downloadPath) => {

        clearTimeout(toastTimeout);

        $(".toast-body").html(`<p>Excel creation process is complete. You can download the excel file from the link below.<p>
        <a href="${downloadPath}">Download</a>
        `);

        $("#liveToast").show();
    })
})
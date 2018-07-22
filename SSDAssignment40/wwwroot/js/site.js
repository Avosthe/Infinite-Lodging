// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


(setTimeout(
        function () {
            let log_alert = document.getElementById("log_alert");
            if (log_alert != null) {
                document.getElementById("close_alert").click();
            }
        }, 10000));
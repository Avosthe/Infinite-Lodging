

window.onload = function () {
    var modal = new RModal(document.getElementById('modal'), {
        //content: 'Abracadabra'
        beforeOpen: function (next) {
            console.log('beforeOpen');
            next();
        },
        afterOpen: function () {
            console.log('opened');
        }

        ,
        beforeClose: function (next) {
            console.log('beforeClose');
            next();
        },
        afterClose: function () {
            console.log('closed');
        }
    });

    document.addEventListener('keydown', function (ev) {
        modal.keydown(ev);
    }, false);

    document.getElementById('showModal').addEventListener("click", function (ev) {
        ev.preventDefault();
        modal.open();
    }, false);
    document.getElementById('showModal').addEventListener("click", function () {
        document.getElementsByClassName("overlay-manage")[0].style.display = "initial";
    });
    document.getElementById('closeModal').addEventListener("click", function () {
        document.getElementsByClassName("overlay-manage")[0].style.display = "none";
        console.log("Done");
    });

    window.modal = modal;
}
var cto = document.getElementById("cto-information");
var cto_img = document.getElementById("cto-img");
var cto_swaggy_img = document.getElementById("cto-swaggy-picture")
var arrayCheck = new Array("", "hidden");

function showHideCTOInformation() {
    if (arrayCheck.includes(cto.style.visibility)) {
        document.querySelector("body").style.overflowY = "hidden";
        cto.style.opacity = "1";
        cto.style.visibility = "visible";
        cto.classList.toggle("cto-slide-right");
        cto_swaggy_img.classList.toggle("cto-zoom-in");
        cto_swaggy_img.style.transform = "scale(1.25)";
    }
    else {
        cto.style.opacity = "0";
        cto.style.visibility = "hidden";
        document.querySelector("body").style.overflowY = "initial";
        cto.classList.toggle("cto-slide-right");
        cto_swaggy_img.classList.toggle("cto-zoom-in");
        document.getElementById("close2").style.opacity = "0";
        cto_swaggy_img.style.transform = "scale(1)";
    }
}

cto_img.addEventListener("click", showHideCTOInformation);

document.getElementById("cto-information").onmousemove = function (e) {
    var close2 = document.getElementById("close2");
    close2.style.opacity = "1";
    close2.style.left = (e.clientX - 25) + "px";
    close2.style.top = (e.clientY - 25) + "px";
}

document.getElementById("cto-information").onclick = showHideCTOInformation;
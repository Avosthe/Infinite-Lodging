var observe;
if (window.attachEvent) {
    observe = function (element, event, handler) {
        element.attachEvent('on' + event, handler);
    };
}
else {
    observe = function (element, event, handler) {
        element.addEventListener(event, handler, false);
    };
}
function init() {
    var text = document.getElementById('text');
    function resize() {
        text.style.height = 'auto';
        text.style.height = text.scrollHeight + 'px';
    }
    /* 0-timeout to get the already changed text */
    function delayedResize() {
        window.setTimeout(resize, 0);
    }
    observe(text, 'change', resize);
    observe(text, 'cut', delayedResize);
    observe(text, 'paste', delayedResize);
    observe(text, 'drop', delayedResize);
    observe(text, 'keydown', delayedResize);

    text.focus();
    text.select();
    resize();
}

function getURL() {
    var url = window.location.href;
    if (url.includes("location=")) {
        var location = url.search("location=");
        location += 9;
        location = url.substring(location);
        while (location.includes("+")) {
            location = location.replace("+", " ")
        }

        if (location == "") {
            document.getElementById("myheader").innerHTML = "Showing all results"
        }
        else {
            document.getElementById("myheader").innerHTML = "Showing results for \"" + location + "\"";
        }
    }
    else {
        document.getElementById("myheader").innerHTML = "Showing all results"
    }
}


function updatePrice() {
    var dateStart = new Date(document.getElementById("dateStart").value);
    var dateEnd = new Date(document.getElementById("dateEnd").value);
    var price = document.getElementById("price").innerHTML;

    var datediff = (dateEnd - dateStart) / (1000 * 60 * 60 * 24);
    var total = datediff * price;


    if (!isNaN(total)) {
        document.getElementById("calculation").innerHTML = "S$" + price + " x " + datediff + " nights";
        document.getElementById("calctotal").innerHTML = "S$" + total;
        document.getElementById("total").innerHTML = "S$" + total;
        document.getElementById("line").style.display = "block";
        document.getElementById("totalprice").style.display = "block";
    }
}

function resizelistingimg() {
    var list = document.getElementsByClassName("listingimg")
    for (var x = 0; x < list.length; x++) {
        if (list[x].clientHeight < 200)
            list[x].style.height = "200px";
    }
}

function updateDate() {
    var dateStart = new Date(document.getElementById("dateStart").value);
    document.getElementById("dateEnd").setAttribute("min", dateStart.getFullYear() + "-" + dateStart.getMonth() + "-" + dateStart.getDate());
    
}

function rating(a) {
    var starList = document.getElementsByClassName("reviewStar");
    document.getElementById("ratingValue").value = a;
    for (var i = 0; i < a; i++) {
        starList[i].innerHTML = "&#9733"
    }

    for (var x = a; x < 5; x++) {
        starList[x].innerHTML = "&#9734"
    }
}

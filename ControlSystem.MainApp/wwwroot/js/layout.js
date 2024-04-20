   document.addEventListener("DOMContentLoaded", function () {
        var currentUrl = window.location.pathname;
        var searchBox = document.querySelector(".search-box");

        if (currentUrl.includes("Workspaces")) {
           searchBox.style.visibility = "visible";
        }
    });
//starting
$(document).ready(function () {
    $(".details").hide();
    $("#basic-info").show();
    $("#basic-info-menu").css({
        "background-color": "#5c3add",
    });
    //calling common event on clicking button
    $(
        "#move-to-infrastructure, #back-to-basicinfo, #move-to-details, #back-to-infrastructure,#move-to-images, #back-to-details"
    ).on("click", function () {
        $("#menu ul li").css("background-color", "#fb432d");
        $(".details").hide();
    });
    //calling common event on clicking menu
    $(
        "#basic-info-menu, #infrastructure-menu, #other-details-menu, #images-menu"
    ).on("click", function () {
        $("#menu ul li").css("background-color", "#fb432d");
        $(this).css({
            "background-color": "#5c3add",
        });
        $(".details").hide();
    });
    //showing the form on click events
    $("#basic-info-menu, #back-to-basicinfo").click(function () {
        $("#basic-info").show();
    });
    $(
        "#infrastructure-menu, #move-to-infrastructure, #back-to-infrastructure"
    ).click(function () {
        $("#infrastructure").show();
    });
    $("#other-details-menu, #move-to-details, #back-to-details").click(
        function () {
            $("#other-details").show();
        }
    );
    $("#images-menu, #move-to-images").click(function () {
        $("#images").show();
    });
    //changing menu color while using buttons
    $("#move-to-infrastructure, #back-to-infrastructure").click(
        function () {
            $("#infrastructure-menu").css("background-color", "#5c3add");
        }
    );
    $("#back-to-basicinfo").click(function () {
        $("#basic-info-menu").css("background-color", "#5c3add");
    });
    $("#move-to-details , #back-to-details").click(function () {
        $("#other-details-menu").css("background-color", "#5c3add");
    });
    $("#move-to-images").click(function () {
        $("#images-menu").css("background-color", "#5c3add");
    });
    // hide and show class to navbar when scrolling 
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        if (scroll >= 20) {
            $(".navbar").addClass("navbar-hide");
        } else {
            $(".navbar").removeClass("navbar-hide");
        }

    });
    
});
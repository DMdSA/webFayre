// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Search for the handle target
document.addEventListener("click", e => {
    let handle
    // If we click on a class="carousel-handler" it will make that our handle
    if (e.target.matches(".carousel-handler")) {
        handle = e.target
    } else {
        // else we click on a text or something inside the handle it will get the parent
        handle = e.target.closest(".carousel-handler")
    }
    //as long as we find something we want to move forward or backwards
    if (handle != null) onHandleClick(handle)
})

//
document.querySelectorAll(".slider-track").forEach(calculateSliderTrack)

//Calculate and create the number of bars on the slider tracking 
//To update the active bar when we click left/right we call it in function onHandleClick
function calculateSliderTrack(sliderTrack) {
    sliderTrack.innerHTML = ""
    //get slider
    const slider = sliderTrack.closest(".slider-row").querySelector(".carousel-slider")
    //number of items in the slider
    const itemCount = slider.children.length
    //get cards-per-screen
    const cardsPerScreen = parseInt(getComputedStyle(slider).getPropertyValue("--cards-per-screen"))
    //get slider index
    const sliderIndex = parseInt(getComputedStyle(slider).getPropertyValue("--slider-index"))
    
    const sliderTrackItemCount = Math.ceil(itemCount / cardsPerScreen)
    for (let i = 0; i < sliderTrackItemCount; i++) {
        const barItem = document.createElement("div")
        barItem.classList.add("track-item")
        //add active status
        if (i === sliderIndex) {
            barItem.classList.add("active)")
        }
        sliderTrack.append(barItem)
    }
}

//Window event listener - Resizing
//When the windows is resized it will recalculate the progress bar
//depending on how many cards are being displayed
window.addEventListener("resize", (e) => {
    //Recalculate
})



//Moving slider left or right
function onHandleClick(handle) {
    const sliderTrack = handle.closest(".slider-row").querySelector(".slider-track")
    const slider = handle.closest(".carousel-container").querySelector(".carousel-slider")
    const sliderIndex = parseInt(getComputedStyle(slider).getPropertyValue("--slider-index"))
    const sliderTrackItemCount = sliderTrack.children.length
    //If the handle class contains "previous" we want to slide forward
    if (handle.classList.contains("previous")) {
        //checking if we can move left and if we cant we cicle back to the end of the items
        if (sliderIndex - 1 < 0) {
            slider.style.setProperty("--slider-index", sliderTrackItemCount - 1)
            //change slider track active
            sliderTrack.children[sliderTrackItemCount - 1].classList.remove("active")
            sliderTrack.children[sliderIndex - 1].classList.add("active")
        } else {
            slider.style.setProperty("--slider-index", sliderIndex - 1)
            //change slider track active
            sliderTrack.children[sliderIndex].classList.remove("active")
            sliderTrack.children[sliderIndex - 1].classList.add("active")
        }
    }
    //If the handle class contains "next" we want to slide forward
    if (handle.classList.contains("next")) {
        //checking if we have more items on the slider
        if (sliderIndex + 1 >= sliderTrackItemCount) {
            //if we do we return to the start
            slider.style.setProperty("--slider-index",0)
            //change slider track active
            sliderTrack.children[sliderIndex].classList.remove("active")
            sliderTrack.children[0].classList.add("active")
        } else {
            //if we arent at the end we just update the active
            slider.style.setProperty("--slider-index", sliderIndex +1)
            //change slider track active
            sliderTrack.children[sliderIndex].classList.remove("active")
            sliderTrack.children[sliderIndex + 1].classList.add("active")
        }
    }
}

function setFooterStyle() {
    var docHeight = $(window).height();
    var footerHeight = $('#footer').outerHeight();
    var footerTop = $('#footer').position().top + footerHeight;
    if (footerTop < docHeight) {
        $('#footer').css('margin-top', (docHeight - footerTop) + 'px');
    } else {
        $('#footer').css('margin-top', '');
    }
    $('#footer').removeClass('invisible');
}
$(document).ready(function () {
    setFooterStyle();
    window.onresize = setFooterStyle;
});
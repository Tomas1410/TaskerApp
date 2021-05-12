// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//    <form asp-action="Index" asp - route - sortType="@ViewData["TypeSortParm"]" asp - route - sortOrder="@ViewData["OrderSortParm"]" >
//    <select name="sort-type" id="sortType">
//        <option value="Date">Date</option>
//        <option value="Alphabetic">Alphabetic</option>
//    </select>
//    <select name="sort-order" id="sortOrder">
//        <option value="Ascending">Ascending</option>
//        <option value="Descending">Descending</option>
//    </select>
//  </form >
const sortInformation = {
    AscendingName: {
        prop: "name",
        direction: 1
    },
    DescendingName: {
        prop: "name",
        direction: -1
    },
    Datep: {
        prop: "date",
        direction: 1
    },
    Alphabetic: {
        prop: "alphabetic",
        direction: -1
    }
};
let sorting = document.querySelector('#sort-type');
sortData(data, sorting.value);
showData(data);
sorting.addEventListener('change', (event) => {
    sortData(data, event.currentTarget.value);
});
function sortData(data, style) {
    // Get the information for the style
    const sort = sortInformation[style];
    if (sort) {
        data.items.sort((a, b) => {
            // Get the values for the property
            const avalue = a[sort.prop];
            const bvalue = b[sort.prop];
            // Compare them
            let result;
            if (typeof avalue === "number") {
                result = avalue - bvalue;
            } else {
                result = String(avalue).localeCompare(bvalue);
            }
            // Return the result, adjusting for ascending/descending
            return sort.direction * result;
        });
        showData(data);
    }
}

function showData(data) {
    const div = document.getElementById("display");
    div.innerHTML = "<ul>" +
        data.items.map(entry => {
            return `<li>${entry.name.replace(/&/g, "&amp;").replace(/</g, "&lt;")} - ${entry.price}</li>`;
        }).join("")
    "</ul>";
}

function setPageination(data) {
    $('#pageination').children().remove();
    if (Math.ceil(data.totalCount / data.pageSize) < 2) {
        $('#loader').css('display', 'none');
        return;
    }
    var currentpage = 1;
    var firstpagedisabled = 'disabled'
    var lastpagedisabled = 'disabled'
    if (data.start > data.pageSize) {
        currentpage = Math.ceil(data.start / data.pageSize);
        firstpagedisabled = ''
    }

    var lastpage = Math.ceil(data.totalCount / data.pageSize)
    if (currentpage != lastpage) {
        lastpagedisabled = '';
    }
    var item = `  <li class="page-item ${firstpagedisabled}">
                    <a class="page-link" onclick="loadlist(this)" data-value="1" tabindex="-1">نخستین</a>
                </li>
                <li class="page-item  ${firstpagedisabled}">
                    <a class="page-link" onclick="loadlist(this)" data-value="${currentpage - 1}" tabindex="-1">
                        <i class="fa fa-hand-o-left" aria-hidden="true"></i>
                    </a>
                </li>`
    if (currentpage - 5 > 1)
        item += `<li class="page-item ">
                    <a class="page-link" onclick="loadlist(this)" data-value="${currentpage - 5}">
                       ...
                    </a>
                </li>`;
    var pagestart = currentpage - 5 >= 1 ? currentpage - 5 : 1;
    var pageend = currentpage + 5 >= lastpage ? lastpage : currentpage + 5;
    for (var i = pagestart; i <= pageend; i++) {
        var currentpageActive = '';
        if (i == currentpage)
            currentpageActive = 'active';

        item += `<li class="page-item ${currentpageActive}">
                            <a class="page-link"  onclick="loadlist(this)" data-value="${i}" >${i}</a></li>`
    }
    if (currentpage + 5 < lastpage)
        item += `<li class="page-item ">
                    <a class="page-link" onclick="loadlist(this)" data-value="${currentpage + 5}">
                       ...
                    </a>
                </li>`;
    item += ` <li class="page-item ${lastpagedisabled}">
                    <a class="page-link" onclick="loadlist(this)" data-value="${currentpage + 1}" tabindex="-1">
                        <i class="fa fa-hand-o-right" aria-hidden="true"></i>
                    </a>
                </li>
                <li class="page-item  ${lastpagedisabled}">
                    <a class="page-link" onclick="loadlist(this)" data-value="${lastpage}" tabindex="-1">  آخرین
                    </a>
                </li>`;

    $('#pageination').append(item);
    window.scrollTo(0, 200);
    $('#loader').css('display', 'none');
}

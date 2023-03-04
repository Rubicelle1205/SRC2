/******************************************************************************
*
* solvento.pagebar version:1.0 
* by Mark Siang
* License: MIT
* 2016 / 1 / 26
*
* requires:
*
*   - jquery.js (http://www.jquery.com) -- tested with 1.8.3
*
* usage:
*	function (userArgs, idx, itemsPerPage, total, callback)
*   
* notes:
*   
******************************************************************************/
if (typeof jQuery != 'undefined') {
    (function ($) {
        $.fn.solventoPageBar = function (userArgs, idx, itemsPerPage, total, callback) {
            var args = {
                containerHtml: "<ul class='clearfix'>{{:firstPageRender}}{{:prePageRender}}{{:pageRender}}{{:nextPageRender}}{{:lastPageRender}}</ul>{{:totalPageRender}}", // render: pagesbarHtml
                firstPageHtml: "<li><a href='#' {{:idx}} class='{{:pageClass}}' aria-label='Previous'><span aria-hidden='true'></span></a></li>",// pageClass: clickEventClass,idx : idx
                lastPageHtml: "<li><a href='#' {{:idx}} class='{{:pageClass}}' aria-label='Next'><span aria-hidden='true'></span></a></li>",// pageClass: clickEventClass,idx : idx
                pageHtml: " <li class='{{:selectedHtml}}'><a {{:idx}} class='{{:pageClass}}' href='#'>{{:pageNumber}}</a></li>",//pageClass: clickEventClass, pageNumber: pageNumber, selectedHtml : selectedHtml,idx : idx
                prePageHtml: "<li><a href='#' {{:idx}} class='{{:pageClass}}' aria-label='Previous'><span aria-hidden='true'>&laquo;</span></a></li>",// pageClass: clickEventClass,idx : idx
                nextPageHtml: "<li><a href='#' {{:idx}} class='{{:pageClass}}' aria-label='Next'><span aria-hidden='true'>&raquo;</span></a></li>",// pageClass: clickEventClass,idx : idx
                totalPageHtml: "<div class='page-intro'>¦@<span>{{:totalCount}}</span>µ§¸ê®Æ</div>",
                pageSelectedHtml: "active",
                pagesPerGroup:10
            }

            // override defaults with user args
            $.extend(true, args, userArgs);

            var tick = new Date().getTime();
            var pageClass = 'pb' + tick;

            var TopObj = this;

            total = parseInt(total);
            itemsPerPage = parseInt(itemsPerPage);
            idx = parseInt(idx);
            args.pagesPerGroup = parseInt(args.pagesPerGroup);

            TopObj.empty();

            if (total == 0)
                return;

            // do nothing if one of the arguments is not a number
            if (isNaN(total) || isNaN(itemsPerPage) || isNaN(idx) || isNaN(args.pagesPerGroup))
                return;

            var pageCount = parseInt(total / itemsPerPage);
            pageCount = total % itemsPerPage != 0 ? pageCount + 1 : pageCount;
            idx = idx > pageCount ? 1 : idx;

            var groupStart = parseInt(idx / args.pagesPerGroup - (idx % args.pagesPerGroup == 0 ? 1 : 0) ) * args.pagesPerGroup + 1;
            var groupEnd = (parseInt(idx / args.pagesPerGroup) + (idx % args.pagesPerGroup == 0 ? 0 : 1) ) * args.pagesPerGroup;

            if (groupEnd > pageCount)
                groupEnd = pageCount;

            var pageHtml = '';
            for (var i = groupStart; i <= groupEnd; i++) {
                pageHtml += args.pageHtml.replace("{{:selectedHtml}}", (i == idx ? args.pageSelectedHtml : ""))
                                         .replace("{{:pageClass}}", pageClass)
                                         .replace("{{:pageNumber}}", i)
                                         .replace("{{:idx}}", "idx='" + i + "'");
            }

            var firstPageHtml = args.firstPageHtml.replace("{{:pageClass}}", pageClass)
                                                  .replace("{{:idx}}", "idx='" + 1 + "'");

            var lastPageHtml = args.lastPageHtml.replace("{{:pageClass}}", pageClass)
                                                  .replace("{{:idx}}", "idx='" + pageCount + "'");

            var prePageHtml = idx != 1 ? args.prePageHtml.replace("{{:pageClass}}", pageClass)
                                                  .replace("{{:idx}}", "idx='" + (idx - 1 == 0 ? 1 : idx - 1) + "'") : '';

            var nextPageHtml = idx < pageCount ? args.nextPageHtml.replace("{{:pageClass}}", pageClass)
                                                  .replace("{{:idx}}", "idx='" + (idx + 1 > pageCount ? pageCount : idx + 1) + "'") : '';

            var totalPageHtml = args.totalPageHtml.replace("{{:totalCount}}",total);

            TopObj.html(args.containerHtml.replace("{{:firstPageRender}}", firstPageHtml)
                                          .replace("{{:prePageRender}}", prePageHtml)
                                          .replace("{{:pageRender}}", pageHtml)
                                          .replace("{{:nextPageRender}}", nextPageHtml)
                                          .replace("{{:lastPageRender}}", lastPageHtml)
                                          .replace("{{:totalPageRender}}", totalPageHtml));

            $("." + pageClass).click(function () {
                callback($(this).attr("idx"));
                return false;
            });
        };
    })(jQuery);
} else {
    alert('jquery javascript library required!');
}



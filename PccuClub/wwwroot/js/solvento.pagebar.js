if (typeof jQuery != 'undefined') {
    (function ($) {
        $.fn.solventoPageBar = function (userArgs, idx, itemsPerPage, total, callback) {
            var args = {
                containerHtml: "<nav aria-label='Page navigation'><ul class='pagination justify-content-end'>{{:firstPageRender}}{{:prePageRender}}{{:pageRender}}{{:nextPageRender}}{{:lastPageRender}}</ul></nav>{{:totalPageRender}}", // render: pagesbarHtml
                firstPageHtml: "<li class='page-item {{:disable}}'><a href='#' {{:idx}} class='page-link {{:pageClass}}' aria-label='Previous'><span aria-hidden='true'></span></a></li>",// pageClass: clickEventClass,idx : idx
                lastPageHtml: "<li class='page-item {{:disable}}'><a href='#' {{:idx}} class='page-link {{:pageClass}}' aria-label='Next'><span aria-hidden='true'></span></a></li>",// pageClass: clickEventClass,idx : idx
                pageHtml: " <li class='page-item {{:selectedHtml}}'><a {{:idx}} class='page-link {{:pageClass}}' href='#'>{{:pageNumber}}</a></li>",//pageClass: clickEventClass, pageNumber: pageNumber, selectedHtml : selectedHtml,idx : idx
                prePageHtml: "<li class='page-item'><a href='#' {{:idx}} class='page-link {{:pageClass}}' aria-label='Previous'><span aria-hidden='true'>&laquo;</span></a></li>",// pageClass: clickEventClass,idx : idx
                nextPageHtml: "<li class='page-item'><a href='#' {{:idx}} class='page-link{{:pageClass}}' aria-label='Next'><span aria-hidden='true'>&raquo;</span></a></li>",// pageClass: clickEventClass,idx : idx
                //totalPageHtml: "<div class='page-intro'>共<span>{{:totalCount}}</span>筆資料</div>",
                totalPageHtml: "",
                pNow: "目前為第 {{:pnow}} 頁 / 共 {{:ptotle}} 頁",
                pEver: "每頁顯示 {{:pever}} 筆 / 共 {{:ptotleever}} 筆",
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
            args.pagesPerGroup = parseInt(itemsPerPage);

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
            var groupEnd = (idx / args.pagesPerGroup < 1 ? 1 : idx / args.pagesPerGroup) + ((idx % args.pagesPerGroup == 0 ? 0 : 1) * args.pagesPerGroup);

            if (pageCount > 10) {
                groupStart = idx;
                groupEnd = groupStart + 9;

                if (groupEnd > pageCount) {
                    groupEnd = pageCount
                    groupStart = groupEnd - 9;
                }
                    
            }
            else {
                groupStart = 1;
                groupEnd = pageCount;
            }

            var pageHtml = '';

            for (var i = groupStart; i <= groupEnd; i++) {
                pageHtml += args.pageHtml.replace("{{:selectedHtml}}", (i == idx ? args.pageSelectedHtml : ""))
                                         .replace("{{:pageClass}}", pageClass)
                                         .replace("{{:pageNumber}}", i)
                                         .replace("{{:idx}}", "idx='" + i + "'");
            }

            
            var firstPageHtml = args.firstPageHtml.replace("{{:pageClass}}", pageClass)
                                                .replace("{{:idx}}", "idx='" + 1 + "'");

            if (idx == 1) {
                firstPageHtml = args.firstPageHtml.replace("{{:pageClass}}", pageClass)
                    .replace("{{:idx}}", "idx='" + 1 + "'").replace("{{:disable}}", "disabled");
            }
            else {
                firstPageHtml = args.firstPageHtml.replace("{{:pageClass}}", pageClass)
                    .replace("{{:idx}}", "idx='" + 1 + "'").replace("{{:disable}}", "");
            }

            var lastPageHtml = args.lastPageHtml.replace("{{:pageClass}}", pageClass)
                                                  .replace("{{:idx}}", "idx='" + pageCount + "'");

            if (idx == pageCount) {
                lastPageHtml = args.lastPageHtml.replace("{{:pageClass}}", pageClass)
                    .replace("{{:idx}}", "idx='" + pageCount + "'").replace("{{:disable}}", "disabled");
            }
            else {
                lastPageHtml = args.lastPageHtml.replace("{{:pageClass}}", pageClass)
                    .replace("{{:idx}}", "idx='" + pageCount + "'").replace("{{:disable}}", "");
            }

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

            if (total > 0) {
                $("#PageNow").css("display", "").html(args.pNow.replace("{{:pnow}}", idx).replace("{{:ptotle}}", pageCount));
                $("#PageEver").css("display", "").html(args.pEver.replace("{{:pever}}", args.pagesPerGroup).replace("{{:ptotleever}}", total));
            }
            else {
                $("#PageNow").css("display", "none");
                $("#PageEver").css("display", "none");
            }
            

            $("." + pageClass).click(function () {
                callback($(this).attr("idx"));
                return false;
            });
        };
    })(jQuery);
} else {
    alert('jquery javascript library required!');
}



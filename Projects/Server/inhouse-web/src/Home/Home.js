import React, { useEffect } from "react";
import "./Home.css"; // 기존 CSS 그대로 사용
import { IonIcon } from "@ionic/react";
import {
  menuOutline,
  folderOutline,
  chevronDownOutline,
  peopleOutline,
  settingsOutline,
  logOutOutline,
  personOutline,
  pricetagsOutline,
  analyticsOutline,
} from "ionicons/icons";

export default function Home() {
  useEffect(() => {
    /* EXPANDER MENU */
    const toggle = document.getElementById("nav-toggle");
    const navbar = document.getElementById("navbar");
    const bodypadding = document.getElementById("body-pd");

    if (toggle && navbar) {
      toggle.addEventListener("click", () => {
        navbar.classList.toggle("expander");
        bodypadding.classList.toggle("body-pd");
      });
    }

    /* LINK ACTIVE */
    const linkColor = document.querySelectorAll(".nav__link");
    function colorLink() {
      linkColor.forEach((l) => l.classList.remove("active"));
      this.classList.add("active");
    }
    linkColor.forEach((l) => l.addEventListener("click", colorLink));

    /* COLLAPSE MENU */
    const linkCollapse = document.getElementsByClassName("collapse__link");
    for (let i = 0; i < linkCollapse.length; i++) {
      linkCollapse[i].addEventListener("click", function () {
        const collapseMenu = this.nextElementSibling;
        collapseMenu.classList.toggle("showCollapse");

        const rotate = collapseMenu.previousElementSibling;
        rotate.classList.toggle("rotate");
      });
    }

    // cleanup 이벤트 리스너
    return () => {
      if (toggle) toggle.removeEventListener("click", () => { });
      linkColor.forEach((l) => l.removeEventListener("click", colorLink));
      for (let i = 0; i < linkCollapse.length; i++) {
        linkCollapse[i].removeEventListener("click", () => { });
      }
    };
  }, []);

  return (
    <div id="body-pd">
      <div className="l-navbar" id="navbar">
        <nav className="nav">
          <div>
            <div className="nav__brand">
              <IonIcon icon={menuOutline} className="nav__toggle" id="nav-toggle" />
            </div>
            <div className="nav__list">
              <a href="#" className="nav__link active">
                <IonIcon icon={personOutline} className="nav__icon" />
                <span className="nav_name">Account</span>
              </a>
              <a href="#" className="nav__link">
                <IonIcon icon={pricetagsOutline} className="nav__icon" />
                <span className="nav_name">Item</span>
              </a>

              <div className="nav__link collapse">
                <IonIcon icon={folderOutline} className="nav__icon" />
                <span className="nav_name">Contents</span>
                <IonIcon icon={chevronDownOutline} className="collapse__link" />
                <ul className="collapse__menu">
                  <a href="#" className="collapse__sublink">Contents1</a>
                  <a href="#" className="collapse__sublink">Contents2</a>
                  <a href="#" className="collapse__sublink">Contents3</a>
                </ul>
              </div>

              <div className="nav__link collapse">
                <IonIcon icon={analyticsOutline} className="nav__icon" />
                <span className="nav_name">Analytics</span>
                <IonIcon icon={chevronDownOutline} className="collapse__link" />
                <ul className="collapse__menu">
                  <a href="#" className="collapse__sublink">Analytics1</a>
                  <a href="#" className="collapse__sublink">Analytics2</a>
                </ul>
              </div>

              <a href="#" className="nav__link collapse">
                <IonIcon icon={peopleOutline} className="nav__icon" />
                <span className="nav_name">Log</span>
              </a>

              <a href="#" className="nav__link">
                <IonIcon icon={settingsOutline} className="nav__icon" />
                <span className="nav_name">ServerManage</span>
              </a>
            </div>
            <a href="#" className="nav__link">
              <IonIcon icon={logOutOutline} className="nav__icon" />
              <span className="nav_name">Log out</span>
            </a>
          </div>
        </nav>
      </div>
    </div>
  );
}

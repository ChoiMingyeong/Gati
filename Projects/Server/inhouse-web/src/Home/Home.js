import React, { useEffect } from "react";
import { Routes, Route, useNavigate } from "react-router-dom";
import "./Home.css";
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

import Account from "../Pages/Account";
import Item from "../Pages/Item";
import ContentA from "../Pages/Contents/ContentA";
import ContentB from "../Pages/Contents/ContentB";
import Traffic from "../Pages/Analytics/Traffic";
import Sales from "../Pages/Analytics/Sales";
import Log from "../Pages/Log";
import ServerManage from "../Pages/ServerManage";

export default function Home() {
  const navigate = useNavigate();

  useEffect(() => {
    // expander menu
    const toggle = document.getElementById("nav-toggle");
    const navbar = document.getElementById("navbar");
    const bodypadding = document.getElementById("body-pd");

    if (toggle && navbar) {
      toggle.addEventListener("click", () => {
        navbar.classList.toggle("expander");
        bodypadding.classList.toggle("body-pd");
      });
    }

    // link active
    const linkColor = document.querySelectorAll(".nav__link");
    function colorLink() {
      linkColor.forEach((l) => l.classList.remove("active"));
      this.classList.add("active");
    }
    linkColor.forEach((l) => l.addEventListener("click", colorLink));

    // collapse menu
    const linkCollapse = document.getElementsByClassName("collapse__link");
    for (let i = 0; i < linkCollapse.length; i++) {
      linkCollapse[i].addEventListener("click", function () {
        const collapseMenu = this.nextElementSibling;
        collapseMenu.classList.toggle("showCollapse");

        const rotate = collapseMenu.previousElementSibling;
        rotate.classList.toggle("rotate");
      });
    }

    // cleanup event listener
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
              <a onClick={() => navigate("/home/account")} className="nav__link active">
                <IonIcon icon={personOutline} className="nav__icon" />
                <span className="nav_name">Account</span>
              </a>
              <a onClick={() => navigate("/home/item")} className="nav__link">
                <IonIcon icon={pricetagsOutline} className="nav__icon" />
                <span className="nav_name">Item</span>
              </a>

              <div className="nav__link collapse">
                <IonIcon icon={folderOutline} className="nav__icon" />
                <span className="nav_name">Contents</span>
                <IonIcon icon={chevronDownOutline} className="collapse__link" />
                <ul className="collapse__menu">
                  <a onClick={() => navigate("/home/contenta")} className="collapse__sublink">Content A</a>
                  <a onClick={() => navigate("/home/contentb")} className="collapse__sublink">Content B</a>
                </ul>
              </div>

              <div className="nav__link collapse">
                <IonIcon icon={analyticsOutline} className="nav__icon" />
                <span className="nav_name">Analytics</span>
                <IonIcon icon={chevronDownOutline} className="collapse__link" />
                <ul className="collapse__menu">
                  <a onClick={() => navigate("/home/traffic")} className="collapse__sublink">Traffic</a>
                  <a onClick={() => navigate("/home/sales")} className="collapse__sublink">Sales</a>
                </ul>
              </div>

              <a onClick={() => navigate("/home/log")} className="nav__link collapse">
                <IonIcon icon={peopleOutline} className="nav__icon" />
                <span className="nav_name">Log</span>
              </a>

              <a onClick={() => navigate("/home/server-manage")} className="nav__link">
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
      {/* show now page */}
      <div style={{ marginLeft: "250px", padding: "20px" }}>
        <Routes>
          <Route path="account" element={<Account />} />
          <Route path="item" element={<Item />} />
          <Route path="contenta" element={<ContentA />} />
          <Route path="contentb" element={<ContentB />} />
          <Route path="traffic" element={<Traffic />} />
          <Route path="sales" element={<Sales />} />
          <Route path="log" element={<Log />} />
          <Route path="server-manage" element={<ServerManage />} />
        </Routes>
      </div>
    </div>
  );
}

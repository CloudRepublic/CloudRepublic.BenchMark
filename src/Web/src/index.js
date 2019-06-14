import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter, Route, Switch, Redirect } from "react-router-dom";

import "assets/vendor/nucleo/css/nucleo.css";
import "assets/vendor/@fortawesome/fontawesome-free/css/all.min.css";
import "assets/scss/argon-dashboard-react.scss";

import BenchMarkLayout from 'layouts/BenchMark.jsx'


ReactDOM.render(
  <BrowserRouter>
    <Switch>
      <Route path="/benchmark" render={props => <BenchMarkLayout {...props} />} />
      <Redirect from="/" to="/benchmark" />
    </Switch>
  </BrowserRouter>,
  document.getElementById("root")
);

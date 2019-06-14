import React, { Component } from "react";
import {
  Button,
  Card,
  CardHeader,
  CardBody,
  CardImg,
  CardTitle,
  CardText,
  Row,
  Col
} from "reactstrap";

// javascipt plugin for creating charts
import Chart from "chart.js";
// react plugin used to create charts
import { Line, Bar } from "react-chartjs-2";
// core components
import {
  chartOptions,
  parseOptions,
  chartExample2
} from "variables/charts.jsx";

export default class EnvironmentBenchMark extends Component {
  componentWillMount() {
    if (window.Chart) {
      parseOptions(Chart, chartOptions());
    }
  }
  render() {
    return (
      <>
        <Row>
          <Col>
            <div style={{ width: "18rem" }}>
              <Card className="card-stats mb-4 mb-lg-0">
                <CardBody>
                  <Row>
                    <div className="col">
                      <CardTitle className="text-uppercase text-muted mb-0">
                        Function {this.props.envName}
                      </CardTitle>
                      <span className="h2 font-weight-bold mb-0">
                        {this.props.AvgPerf}
                      </span>
                    </div>
                    <Col className="col-auto">
                      <div className="icon icon-shape bg-danger text-white rounded-circle shadow" />
                    </Col>
                  </Row>
                  <p className="mt-3 mb-0 text-muted text-sm">
                    <span className="text-success mr-2">
                      {this.props.lastDayDiff}
                    </span>
                    <span className="text-nowrap">Since last 3 days</span>
                  </p>
                </CardBody>
              </Card>
            </div>
          </Col>
        </Row>

        <Row className="mt-3">
          <Col md="6">
            <Card>
              <CardBody>
                <div className="chart">
                  {/* Chart wrapper */}
                  <Line
                    data={chartExample2.data}
                    options={chartExample2.options}
                  />
                </div>
              </CardBody>
            </Card>
          </Col>
          <Col md="6">
            <Card>
              <CardBody>
                <div className="chart">
                  {/* Chart wrapper */}
                  <Line
                    data={chartExample2.data}
                    options={chartExample2.options}
                  />
                </div>
              </CardBody>
            </Card>
          </Col>
        </Row>
      </>
    );
  }
}

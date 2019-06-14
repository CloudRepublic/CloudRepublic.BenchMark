import React from "react";

import EnvironmentBenchMark from "components/BenchMark/EnvironmentBenchMark.jsx";

// reactstrap components
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

class Index extends React.Component {
  constructor() {
    super();
    this.state = {
      windowsCsharpEnvironments: {}
    };
  }
 async componentDidMount() {
    fetch(
      "https://benchmark-backend-api.azurewebsites.net/api/Trigger?code=QvUwR7CI64qYR8JYH/epnBG/K80EOvYtibszl4VHzT6hSfXzRauAXA=="
    )
      .then(res => {
        return res.json();
      })
      .then(data => {
        let windowsEnvironments = data.cloudProviders[0].hostingEnvironments.map(
          hostingEnvi => {
            if(hostingEnvi.name === 'Windows') return hostingEnvi;
          }
        );

        console.log(windowsEnvironments)
      });
  }
  render() {
    return (
      <>
        <Row>
          <Col md="12">
            <Row>
              <Col md="5" className="d-flex flex-column">
                <span className="display-1 text-white">Azure</span>
                <span className="display-4 text-mute">Csharp</span>
              </Col>
              <Row>
                <Col md="12" className="mt-3">
                  <EnvironmentBenchMark envName="Windows" />
                </Col>
              </Row>
              
            </Row>
          </Col>
        </Row>
        <Row>
          <Col md="12">
            <Row>
              <Col md="5" className="d-flex flex-column">
                <span className="display-1 text-white">Azure</span>
                <span className="display-4 text-mute">Nodejs</span>
              </Col>
            </Row>
          </Col>
        </Row>
      </>
    );
  }
}

export default Index;

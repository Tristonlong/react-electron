/**
 * Created by hao.cheng on 2017/5/7.
 */
import React from "react";

export default class NotFound extends React.Component {
  state = {
    animated: "",
  };
  enter = () => {
    document.title = "404";
    this.setState({ animated: "hinge" });
  };

  render() {
    const img = "../../style/imgs/404.png";
    return (
      <div
        className="center"
        style={{ height: "100%", background: "#ececec", overflow: "hidden" }}
      >
        <img
          src={img}
          alt="404"
          className={`animated swing ${this.state.animated}`}
          onMouseEnter={this.enter}
        />
      </div>
    );
  }
}

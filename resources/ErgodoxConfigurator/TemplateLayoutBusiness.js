import { types as t } from "mobx-state-tree";

const KEY_UNIT_SIZE = 54;
const KEY_TOP_COLOR = '#fcfcfc';
const KEY_BASE_COLOR = '#f4f4f4';
const KEY_BORDER_COLOR = '#c7c7c7';
const KeyPosition = t
  .model({
    x: t.number,
    y: t.number,
    width: t.optional(t.number, 1),
    height: t.optional(t.number, 1),
    voffset: t.optional(t.number, 0),
    hoffset: t.optional(t.number, 0),
    rotate: t.optional(t.number, 0),
    origin: t.optional(t.string, "top left"),
    glow: false
  })
  .views(self => ({
    get styles() {
      const { x, y, width, height, voffset, hoffset, rotate, origin } = self;
      const widthInPixel = width * KEY_UNIT_SIZE;
      const heightInPixel = height * KEY_UNIT_SIZE;
      const keyBaseTop =
        ((voffset * KEY_UNIT_SIZE + y * KEY_UNIT_SIZE) / 415) * 100;
      const keyBaseLeft =
        ((hoffset * KEY_UNIT_SIZE + x * KEY_UNIT_SIZE) / 1051) * 100;
      const keyBaseWidth = (widthInPixel / 1083) * 100;
      const keyBaseHeight = (heightInPixel / 415) * 100;
      const keyBase = {
        top: `${keyBaseTop}%`,
        left: `${keyBaseLeft}%`,
        width: `${keyBaseWidth}%`,
        height: `${keyBaseHeight}%`,
        transform: `rotate(${rotate}deg)`,
        transformOrigin: origin
      };

      const keyTop = {
        width: `${((widthInPixel - 3 - 3) / widthInPixel) * 100}%`,
        height: `${((heightInPixel - 3 - 8) / heightInPixel) * 100}%`,
        margin: `${(3 / widthInPixel) * 100}%`,
        marginBottom: `${(8 / widthInPixel) * 100}%`
      };

      return {
        keyBase,
        keyTop
      };
    }
  }));

export default KeyPosition;
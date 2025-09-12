export {};

declare global {
  type GameToSave = {
    help: number;
    seconds: number;
    difficulty: string;
    rows: number;
    cols: number;
    mines: number;
    n3BV: number;
    clicks: number;
    efficiency: number;
    hash: string;
  };
}
export interface DonCaNhanRequest {
  LoaiDK?: string; // Luôn là 'CANHAN' trong method  InsertOnlynày >
  CaAn: number; // Ca ăn: "1", "2", "3" (dạng so)
  SoLuong: number; // Mặc định đăng ký 1 suất
}

export enum TrangThaiDon {
  ChoXacNhan = 0,
  DaXacNhan = 1,
}

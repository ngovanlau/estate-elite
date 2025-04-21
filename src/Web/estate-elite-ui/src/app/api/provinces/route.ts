import { NextResponse } from 'next/server';
import fs from 'fs';
import path from 'path';
import { Province } from '@/types';

export async function GET() {
  try {
    const provinces = getProvinces();
    return NextResponse.json(provinces);
  } catch (error) {
    console.error('Error fetching provinces:', error);
    return NextResponse.json({ error: 'Failed to fetch provinces' }, { status: 500 });
  }
}

const getProvinces = (): Province[] => {
  const filePath = path.join(
    process.cwd(),
    '/src/public/json/simplified_json_generated_data_vn_units_minified.json'
  );

  const fileData = fs.readFileSync(filePath, 'utf-8');
  const provinces: Province[] = JSON.parse(fileData);
  return provinces;
};

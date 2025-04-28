'use client';

import { useState } from 'react';
import Image from 'next/image';
import { Button } from '@/components/ui/button';
import { ArrowLeft, ArrowRight } from 'lucide-react';

interface PropertyDetailsGalleryProps {
  images: string[];
}

const PropertyDetailsGallery = ({ images }: PropertyDetailsGalleryProps) => {
  const [activeIndex, setActiveIndex] = useState(0);

  const goToPrevious = () => {
    setActiveIndex((current) => (current === 0 ? images.length - 1 : current - 1));
  };

  const goToNext = () => {
    setActiveIndex((current) => (current === images.length - 1 ? 0 : current + 1));
  };

  return (
    <div className="relative">
      <div className="relative h-96 overflow-hidden rounded-lg md:h-[500px]">
        <Image
          src={images[activeIndex]}
          alt="Property image"
          fill
          className="object-cover"
        />

        <div className="absolute inset-0 flex items-center justify-between px-4">
          <Button
            variant="outline"
            size="icon"
            className="rounded-full bg-white/70 hover:bg-white"
            onClick={goToPrevious}
          >
            <ArrowLeft className="h-5 w-5" />
          </Button>

          <Button
            variant="outline"
            size="icon"
            className="rounded-full bg-white/70 hover:bg-white"
            onClick={goToNext}
          >
            <ArrowRight className="h-5 w-5" />
          </Button>
        </div>
      </div>

      <div className="mt-4 grid grid-cols-5 gap-2">
        {images.map((image, index) => (
          <div
            key={index}
            className={`relative h-20 cursor-pointer overflow-hidden rounded-md transition-all ${
              index === activeIndex ? 'ring-2 ring-blue-600' : ''
            }`}
            onClick={() => setActiveIndex(index)}
          >
            <Image
              src={image}
              alt={`Thumbnail ${index + 1}`}
              fill
              className="object-cover"
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default PropertyDetailsGallery;
